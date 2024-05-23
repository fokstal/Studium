import { Group } from "./Group";
import { Person } from "./Person";

export type Student = {
  id?: number;
  person: Person;
  personId?: number;
  group: Group;
  groupId?: number;
  addedDate?: string;
  removedDate?: string;
}