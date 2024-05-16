import { Student } from "./Student";
import { Subject } from "./Subject";

export type Grade = {
  id?: number;
  value: number;
  setDate: Date | string;
  studentId: number;
  subjectId: number;
  student?: Student;
  subject?: Subject;
};
