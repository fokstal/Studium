import { PersonService, StudentService, SubjectService } from "../services";
import { Grade, Group, Person, Student, Subject } from "../types";

const studentService = new StudentService();
const subjectService = new SubjectService();
const personService = new PersonService();

type Filter = {
  group?: Group;
  person?: Person;
  subject?: Subject;
};

export async function formatGrades(filters: Filter, grades?: Grade[]) {

  const students = await studentService.get();
  const subjects = await subjectService.get();
  const persons = await personService.get();
  const formattedGrades: any[] = [];

  const gradeFormat = (person?: Person, subject?: Subject) => {
    const student = students.find((s: Student) => s.personId === person?.id);

    const formattedGrade = {};
    if (subject && !person) {
      Object.defineProperty(formattedGrade, "name", { value: subject.name });
    }

    if (person && !subject) {
      Object.defineProperty(formattedGrade, "id", { value: student.id });
      Object.defineProperty(formattedGrade, "name", {
        value: `${person.firstName} ${person.lastName}`,
      });
    }

    grades?.forEach(({ setDate, value, studentId, subjectId }, index) => {
      const date = new Date(setDate);
      const beutifullDate = `${date.getDate()}.${date.getMonth()}_${index}`;

      if (person && !subject) {
        if (studentId === student.id) {
          Object.defineProperty(formattedGrade, beutifullDate, {
            value: value,
          });
        }
      }
      if (!person && subject) {
        if (subjectId === subject.id) {
          Object.defineProperty(formattedGrade, beutifullDate, {
            value: value,
          });
        }
      }

      if (person && subject) {
        if (subjectId === subject.id && studentId === student.id) {
          Object.defineProperty(formattedGrade, beutifullDate, {
            value: value,
          });
        }
      }
    });
    formattedGrades.push(formattedGrade);
  };

  if (!filters) return[];
  if (filters.person && filters.subject) {
    gradeFormat(filters.person, filters.subject);
  } else if (filters.person) {
    const groupId = students.find(
      (s: Student) => s.personId === filters.person?.id
    ).groupId;
    subjects
      .filter((s: Subject) => s.groupId === groupId)
      .forEach((s: Subject) => gradeFormat(undefined, s));
  } else if (filters.subject) {
    students
      .filter((s: Student) => s.groupId === subjects.groupId)
      .forEach((s: Student) =>
        gradeFormat(persons.find((p: Person) => s.personId === p.id))
      );
  } else if (filters.group) {
    students
      .filter((s: Student) => s.groupId === filters.group?.id)
      .forEach((s: Student) =>
        gradeFormat(persons.find((p: Person) => s.personId === p.id))
      );
  } else {
    students.forEach((s: Student) =>
      gradeFormat(persons.find((p: Person) => s.personId === p.id))
    );
  }
  return formattedGrades;
}
