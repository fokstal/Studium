import { PersonService, StudentService } from "../../services";
import { Person, Student } from "../../types";
import { objectToFormData } from "../objToFormData";

export async function createStudent(studentData: Student & Person) {
  const studentService = new StudentService();
  const personService = new PersonService();

  const person = await personService
    .post(objectToFormData({
      firstName: studentData.firstName,
      middleName: studentData.middleName,
      lastName: studentData.lastName,
      birthDate: studentData.birthDate,
      avatar: studentData.avatarFileName,
      sex: studentData.sex,
    }))
    .catch((err) => {
      throw new Error(err);
    });

  await studentService
    .post({
      personId: person.id,
      groupId: studentData.group.id,
    })
    .catch((err) => {
      throw new Error(err);
    });
}
