import { PassportService, PersonService, StudentService } from "../../services";
import { Person, Student } from "../../types";
import { objectToFormData } from "../objToFormData";

export async function editStudent(
  studentData: Student & Person,
  passport?: File
) {
  const studentService = new StudentService();
  const personService = new PersonService();
  const passportService = new PassportService();

  if (!studentData.personId || !studentData.id) return;

  const person = await personService
    .put(
      studentData.personId,
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
    .put(studentData.id, {
      personId: studentData.personId,
      groupId: studentData?.group?.id,
      addedDate: studentData.addedDate,
      removedDate: studentData.removedDate,
    })
    .catch((err) => {
      throw new Error(err);
    });

  if (passport) {
    const passports = await passportService.get();
    const passportId = passports.find(
      (p: any) => p.personId === studentData.personId
    );

    if (passportId) {
      await passportService
        .put(
          passportId,
          objectToFormData({ personId: studentData.personId, scan: passport })
        )
        .catch((err) => {
          throw new Error(err);
        });
    } else {
      await passportService
        .post(
          objectToFormData({ personId: studentData.personId, scan: passport })
        )
        .catch((err) => {
          throw new Error(err);
        });
    }
  }
}
