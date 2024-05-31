import { PassportService, PersonService, StudentService } from "../../services";
import { Person, Student } from "../../types";
import { objectToFormData } from "../objToFormData";

export async function createStudent(
  studentData: Student & Person,
  passport?: File
) {
  const studentService = new StudentService();
  const personService = new PersonService();
  const passportService = new PassportService();

  const person = await personService
    .post(
      objectToFormData({
        firstName: studentData.firstName,
        middleName: studentData.middleName,
        lastName: studentData.lastName,
        birthDate: studentData.birthDate,
        avatar: studentData.avatarFileName,
        sex: studentData.sex,
      })
    )
    .catch((err) => {
      throw new Error(err);
    });

  await studentService
    .post({
      personId: person.id,
      groupId: studentData?.group?.id,
      addedDate: studentData.addedDate,
      removedDate: studentData.removedDate,
    })
    .catch((err) => {
      throw new Error(err);
    });
  if (passport) {
    await passportService
      .post(objectToFormData({ personId: person.id, scan: passport }))
      .catch((err) => {
        throw new Error(err);
      });
  }
}
