import { Student } from "./Student";
import { Subject } from "./Subject";

export type Grade = {
  id?: number;
  value: number;
  setDate: Date | string;
  studentEntityId: number;
  subjectEntityId: number;
  student?: Student;
  subject?: Subject;
};
