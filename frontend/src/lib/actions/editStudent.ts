import { PersonService, StudentService } from "../../services";
import { Person, Student } from "../../types";
import { objectToFormData } from "../objToFormData";

export async function editStudent(studentData: Student & Person) {
  const studentService = new StudentService();
  const personService = new PersonService();

  if (!studentData.personId || !studentData.id) return;

  await personService
    .put(studentData.personId, objectToFormData({
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
    .put(studentData.id, {
      personId: studentData.personId,
      groupId: studentData.group.id,
    })
    .catch((err) => {
      throw new Error(err);
    });
}