import { Group } from "./Group";
import { Person } from "./Person";

export type Student = {
  id?: number;
  person?: Person;
  personEntityId?: number;
  group?: Group;
  groupEntityId?: number;
  addedDate?: string;
  removedDate?: string;
}