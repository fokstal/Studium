import { PageLayout } from "../../layouts";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import "./default.css";
import { CreateEditStudent, Error404Page, Home, Profile, Student, Students } from "../../pages";
import { Journal } from "../../pages/journal";

export function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route index element={<Home/>}/>
        <Route path="/" element={<PageLayout/>}>
          <Route path="journal" element={<Journal/>}/>
          <Route path="students" element={<Students/>}/>
          <Route path="students/new" element={<CreateEditStudent/>}/>
          <Route path="students/:id" element={<Student/>}/>
          <Route path="profile" element={<Profile/>}/>
        </Route>
        <Route path="*" element={<Error404Page/>}/>
      </Routes>
    </BrowserRouter>
  );
}

