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
};

export async function formatGrades(filters: Filter) {
  let grades: any = [];
  let columns: any = [];
  let tableData = [];
  if (filters?.person && filters?.subject) {
    const students = await studentService.get();
    const student = students.find(
      (s: Student) => s.personId === filters.person?.id
    );
    grades = await gradeService.getBySubjectByStudent(
      filters?.subject?.id || 0,
      student.id || 0
    );

    columns = [
      ...grades.map((g: Grade) => ({
        field: g.setDate.toString(),
        header: new Date(g.setDate).toLocaleDateString(),
      })),
    ];

    const data = {};
    grades.forEach((g: Grade) => {
      Object.defineProperty(data, g.setDate.toString(), { value: g.value });
    });
    tableData = [data];
  } else if (filters?.person) {
    const students = await studentService.get();
    const subjects = await subjectService.get();
    const student = students.find(
      (s: Student) => s.personId === filters.person?.id
    );
    const studentSubjects = subjects.filter(
      (s: Subject) => s.groupId === student.groupId
    );

    grades = await gradeService.getByStudent(student.id);

    columns = [
      { field: "subject", header: "Предмет" },
      ...grades.map((g: Grade) => ({
        field: g.setDate.toString(),
        header: new Date(g.setDate).toLocaleDateString(),
      })),
    ];
    const data = studentSubjects.map((subject: Subject) => {
      const result = {
        subject: subject.name,
      };
      grades.forEach((g: Grade) => {
        if (subject.id !== g.subjectId) return;
        Object.defineProperty(result, g.setDate.toString(), { value: g.value });
      });
      return result;
    });

    tableData = data;
  } else if (filters?.subject) {

    grades = await gradeService.getBySubject(filters.subject?.id || 0);
    columns = [
      { field: "student", header: "Студент" },
      ...grades.map((g: Grade) => ({
        field: g.setDate.toString(),
        header: new Date(g.setDate).toLocaleDateString(),
      })),
    ];

    const students = await studentService.get();
    const neededStudents = students.filter(
      (s: Student) => s.groupId === filters.subject?.groupId
    );

    const data = await Promise.all(
      neededStudents.map(async (s: Student) => {
        const student = await personService.getById(s.personId || 0);
        const result: any = {
          student: student.firstName + " " + student.lastName,
        };

        const studentGrades = grades.filter(
          (g: any) => g.studentToValueList[0].studentId === s.id
        );
        studentGrades.forEach((g: any) => {
          Object.defineProperty(result, g.setDate.toString(), {
            value: g.studentToValueList[0].value,
          });
        });

        return result;
      })
    );
    tableData = data;
  } 
  return {columns, tableData};
}
