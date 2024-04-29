import { PageLayout } from "../../layouts";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import "./default.css";
import { Error404Page, Home, Students } from "../../pages";

export function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route index element={<Home/>}/>
        <Route path="/" element={<PageLayout/>}>
          <Route path="journal" element={<>Эл журнал</>}/>
          <Route path="students" element={<Students/>}/>
          <Route path="students/:id" element={<>Студент</>}/>
          <Route path="profile" element={<>Профиль</>}/>
        </Route>
        <Route path="*" element={<Error404Page/>}/>
      </Routes>
    </BrowserRouter>
  );
}

