import { Flex } from "@chakra-ui/react";
import { Select } from "../ui-kit";
import { useEffect, useState } from "react";
import { Group, Person, Student, Subject } from "../../types";
import {
  GroupService,
  PersonService,
  StudentService,
  SubjectService,
} from "../../services";

const groupService = new GroupService();
const personService = new PersonService();
const subjectService = new SubjectService();
const studentService = new StudentService();

type JournalFiltersProps = {
  filters: { group?: Group; person?: Person; subject?: Subject };
  setFilters: Function;
};

export function JournalFilters({ filters, setFilters }: JournalFiltersProps) {
  const [selectOptions, setSelectOptions] = useState<{
    groups?: Group[];
    persons?: Person[];
    subjects?: Subject[];
    students?: Student[];
  }>();

  const { group, person, subject } = filters;

  const setGroup = (group: Group) => setFilters({ ...filters, group });
  const setPerson = (person: Person) => setFilters({ ...filters, person });
  const setSubject = (subject: Subject) => setFilters({ ...filters, subject });

  const updateOptions = async () => {
    const groups = await groupService.get();
    const persons = await personService.get();
    const subjects = await subjectService.get();
    const students = await studentService.get();

    const truePersons: Person[] = students.map((s: Student) =>
      persons?.find((p: Person) => p.id === s.personId)
    );

    setSelectOptions({
      groups,
      persons: truePersons,
      subjects,
      students,
    });
  };

  useEffect(() => {
    updateOptions();
  }, []);

  const personOptions = () => {
    if (!group) return selectOptions?.persons;

    return selectOptions?.students
      ?.filter((s) => s.groupId === group.id)
      .map((s) => selectOptions.persons?.find((p) => p.id === s.personId));
  };

  const subjectOptions = () => {
    if (!group) return selectOptions?.subjects;

    return selectOptions?.subjects?.filter((s) => s.groupId === group.id);
  };

  return (
    <Flex gap="20px" align="center">
      <Select
        placeholder="Выберите группу"
        options={selectOptions?.groups || []}
        value={group}
        setValue={setGroup}
      />
      <Select
        placeholder="Выберите учащегося"
        options={personOptions() || []}
        value={person}
        setValue={setPerson}
        name="firstName"
      />
      <Select
        placeholder="Выберите предмет"
        options={subjectOptions() || []}
        value={subject}
        setValue={setSubject}
      />
    </Flex>
  );
}
