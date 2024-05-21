import { PageLayout } from "../../layouts";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import "./default.css";
import {
  CreateEditGroup,
  CreateEditStudent,
  CreateEditSubject,
  Error404Page,
  Groups,
  Home,
  Profile,
  Student,
  Students,
} from "../../pages";
import { Journal } from "../../pages/journal";

export function App() {
  return (
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
          <Route path="group/new" element={<CreateEditGroup />} />
          <Route path="subject/new" element={<CreateEditSubject />} />
        </Route>
        <Route path="*" element={<Error404Page />} />
      </Routes>
    </BrowserRouter>
  );
}
