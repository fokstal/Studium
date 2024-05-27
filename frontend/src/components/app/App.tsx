import { PageLayout } from "../../layouts";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import "./default.css";
import {
  CreateGroup,
  CreateEditStudent,
  CreateEditSubject,
  Error404Page,
  Groups,
  Home,
  Profile,
  Student,
  Students,
  Subjects,
} from "../../pages";
import { Journal } from "../../pages/journal";
import { useState } from "react";
import { LanguageContext } from "../../store";
import { CreateUser, Users } from "../../pages/users";

export function App() {
  const [lang, setLang] = useState<{ name: "RU" | "BE" | "EN" }>({
    name: "RU",
  });

  return (
    <LanguageContext.Provider
      value={{ lang: lang || { name: "RU" }, setLang: setLang }}
    >
      <BrowserRouter>
        <Routes>
          <Route index element={<Home />} />
          <Route path="/" element={<PageLayout />}>
            <Route path="students" element={<Students />} />
            <Route path="journal" element={<Journal />} />
            <Route path="students/new" element={<CreateEditStudent />} />
            <Route path="students/:id" element={<Student />} />
            <Route path="profile" element={<Profile />} />
            <Route path="group" element={<Groups />} />
            <Route path="group/new" element={<CreateGroup />} />
            <Route path="group/:id" element={<CreateGroup />} />
            <Route path="subject" element={<Subjects />} />
            <Route path="subject/new" element={<CreateEditSubject />} />
            <Route path="users/" element={<Users />} />
            <Route path="users/new" element={<CreateUser />} />
          </Route>
          <Route path="*" element={<Error404Page />} />
        </Routes>
      </BrowserRouter>
    </LanguageContext.Provider>
  );
}
