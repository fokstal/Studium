import { generateKeyPair } from "crypto";
import {
  GradeService,
  PersonService,
  StudentService,
  SubjectService,
} from "../services";
import { Grade, Group, Person, Student, Subject } from "../types";

const gradeService = new GradeService();
const studentService = new StudentService();
const subjectService = new SubjectService();
const personService = new PersonService();

type Filter = {
  group?: Group;
  person?: Person;
  subject?: Subject;
  studentId?: string;
};

export async function formatGrades(filters: Filter) {
  let grades: any = [];
  let columns: any = [];
  let tableData = [];
  if ((filters?.person || filters?.studentId) && filters?.subject) {
    if (filters.studentId) {
      grades = await gradeService.getBySubjectByStudent(
        filters?.subject?.id || 0,
        filters?.studentId || ""
      );
    } else {
      const students = await personService.getPersonsBySession();
      const student = students.find(
        (p: Person) => p.id === filters.person?.id
      );
      grades = await gradeService.getBySubjectByStudent(
        filters?.subject?.id || 0,
        student.studentEntity.id || 0
      );
    }

    columns = [
      ...grades.map((g: Grade, i: number) => ({
        field: g.setDate.toString() + i,
        header: new Date(g.setDate).toLocaleDateString(),
      })),
    ];

    const data = {};
    grades.forEach((g: Grade, i: number) => {
      Object.defineProperty(data, g.setDate.toString() + i, { value: g.value });
    });
    tableData = [data];
  } else if (filters?.person) {
    const persons = await personService.getPersonsBySession();
    const subjects = await subjectService.getSubjectsBySession();
    const person = persons.find(
      (p: Person) => p.id === filters.person?.id
    );
    const studentSubjects = subjects.filter(
      (s: Subject) => s.groupEntityId === person.studentEntity.groupEntityId
    );

    grades = await gradeService.getByStudent(person.studentEntity.id);

    columns = [
      { field: "subject", header: "Предмет" },
      ...grades.map((g: Grade, i: number) => ({
        field: g.setDate.toString() + i,
        header: new Date(g.setDate).toLocaleDateString(),
      })),
    ];
    const data = studentSubjects.map((subject: Subject) => {
      const result = {
        subject: subject.name,
      };
      grades.forEach((g: Grade, i: number) => {
        if (subject.id !== g.subjectEntityId) return;
        Object.defineProperty(result, g.setDate.toString() + i, {
          value: g.value,
        });
      });
      return result;
    });

    tableData = data;
  } else if (filters?.subject) {
    grades = await gradeService.getBySubject(filters.subject?.id || 0);
    columns = [
      { field: "student", header: "Студент" },
      ...grades.map((g: Grade, i: number) => ({
        field: g.setDate.toString(),
        header: new Date(g.setDate).toLocaleDateString(),
      })),
    ];

    const persons = await personService.getPersonsBySession();
    const students = persons.map((p: Person) => p.studentEntity);
    const neededStudents = students.filter(
      (s: Student) => s.groupEntityId === filters.subject?.groupEntityId
    );

    const data = await Promise.all(
      neededStudents.map(async (s: Student) => {
        const student = await personService.getById(s.personEntityId || 0);
        const result: any = {
          student: student.firstName + " " + student.lastName,
        };

        const studentGrades = grades.filter((g: any) =>
          g.gradeEntityList.find((gr: any) => gr.studentEntityId === s.id)
        );
        studentGrades.forEach((g: any) => {
          Object.defineProperty(result, g.setDate.toString(), {
            value: g.gradeEntityList.find(
              (gr: any) => gr.studentEntityId === s.id
            ).value,
          });
        });

        return result;
      })
    );
    tableData = data;
  }
  return { columns, tableData };
}
