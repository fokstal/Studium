import { Group } from "../types";

export function filterStudents(students: any, filters: {
  group: Group | null;
  startDate: string | null;
  endDate: string | null;
  finish: boolean;
  start: boolean;
}, search: string) {
  return students.filter((student: any) => {
    const studentAddedDate = new Date(student.addedDate);
    const studentRemovedDate = new Date(student.removedDate);

    if (filters.group && student.groupId !== filters.group.id) {
      return false;
    }

    if (filters.startDate && filters.endDate) {
      const startFilterDate = new Date(filters.startDate);
      const endFilterDate = new Date(filters.endDate);
      if (studentAddedDate < startFilterDate || studentAddedDate > endFilterDate) {
        return false;
      }
    } else if (filters.startDate) {
      const startFilterDate = new Date(filters.startDate);
      if (studentAddedDate < startFilterDate) {
        return false;
      }
    } else if (filters.endDate) {
      const endFilterDate = new Date(filters.endDate);
      if (studentAddedDate > endFilterDate) {
        return false;
      }
    }

    if (filters.start && studentAddedDate.getFullYear() !== new Date().getFullYear()) {
      return false;
    }
    if (filters.finish && studentRemovedDate.getFullYear() !== new Date().getFullYear()) {
      return false;
    }

    if (search && !student.name.toLowerCase().includes(search.toLowerCase())) {
      return false;
    }

    return true;
  });
}
