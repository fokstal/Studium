import { Flex, Text, useDisclosure } from "@chakra-ui/react";
import { Button, Select } from "../ui-kit";
import { useContext, useEffect, useState } from "react";
import { Group, Person, Student, Subject } from "../../types";
import {
  GroupService,
  PersonService,
  StudentService,
  SubjectService,
} from "../../services";
import { GradeModal } from "../modal";
import { LanguageContext, Translator } from "../../store";
import { useRoles } from "../../hooks";
import { AiOutlineClose } from "react-icons/ai";

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
  }>({});
  const { isOpen, onOpen, onClose } = useDisclosure();
  const { lang } = useContext(LanguageContext);
  const roles = useRoles();

  const { group, person, subject } = filters;

  const setGroup = (group: Group) => setFilters({ ...filters, group });
  const setPerson = (person: Person) => setFilters({ ...filters, person });
  const setSubject = (subject: Subject) => setFilters({ ...filters, subject });

  const updateOptions = async () => {
    if (roles.includes("Student")) {
      const subjects = await subjectService.getSubjectsBySession();
      return setSelectOptions({ subjects: subjects });
    } else {
      const groups = await groupService.get();
      const persons = await personService.get();
      const subjects = await subjectService.getSubjectsBySession();
      const students = await studentService.get();

      const truePersons: Person[] = students.map((s: Student) =>
        persons?.find((p: Person) => p.id === s.personEntityId)
      );

      setSelectOptions({
        groups,
        persons: truePersons,
        students,
        subjects
      });
    }
  };

  useEffect(() => {
    updateOptions();
  }, []);

  const personOptions = () => {
    if (!group) return selectOptions?.persons;

    return selectOptions?.students
      ?.filter((s) => s.groupEntityId === group.id)
      .map((s) =>
        selectOptions.persons?.find((p) => p.id === s.personEntityId)
      );
  };

  const subjectOptions = () => {
    if (!group) return selectOptions?.subjects;

    return selectOptions?.subjects?.filter((s) => s.groupEntityId === group.id);
  };

  return (
    <Flex gap="20px" align="center">
      {roles.includes("Student") ? null : (
        <>
          <Select
            placeholder={Translator[lang.name]["select_group"]}
            options={selectOptions?.groups || []}
            value={group}
            setValue={setGroup}
          />
          <Select
            placeholder={Translator[lang.name]["select_student"]}
            options={personOptions() || []}
            value={person}
            disabled={!filters.group}
            setValue={setPerson}
            name="lastName"
          />
        </>
      )}
      <Select
        placeholder={Translator[lang.name]["select_subject"]}
        options={subjectOptions() || []}
        disabled={roles.includes("Student") ? false : !filters.group}
        value={subject}
        setValue={setSubject}
      />
      {roles.includes("Student") || roles.includes("Curator") ? null : (
        <Button onClick={onOpen}>{Translator[lang.name]["add_mark"]}</Button>
      )}
      <Flex
        gap="10px"
        align="center"
        cursor="pointer"
        onClick={() => setFilters({})}
      >
        <Text userSelect="none">{Translator[lang.name]["clear_filters"]}</Text>
        <AiOutlineClose />
      </Flex>
      {roles.includes("Student") || roles.includes("Curator") ? null : (
        <GradeModal onClose={onClose} isOpen={isOpen} />
      )}
    </Flex>
  );
}
