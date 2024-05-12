import { PageLayout } from "../../layouts";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import "./default.css";
import { CreateEditStudent, Error404Page, Home, Student, Students } from "../../pages";

export function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route index element={<Home/>}/>
        <Route path="/" element={<PageLayout/>}>
          <Route path="journal" element={<>Эл журнал</>}/>
          <Route path="students" element={<Students/>}/>
          <Route path="students/new" element={<CreateEditStudent/>}/>
          <Route path="students/:id" element={<Student/>}/>
          <Route path="profile" element={<>Профиль</>}/>
        </Route>
        <Route path="*" element={<Error404Page/>}/>
      </Routes>
    </BrowserRouter>
  );
}

