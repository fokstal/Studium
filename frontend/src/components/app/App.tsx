import { PageLayout } from "../../layouts/page-layout/PageLayout";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import "./default.css";

export function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route index element={<>Home</>}/>
        <Route path="/" element={<PageLayout/>}>
          <Route path="journal" element={<>Эл журнал</>}/>
          <Route path="students" element={<>Студенты</>}/>
          <Route path="students/:id" element={<>Студент</>}/>
          <Route path="profile" element={<>Профиль</>}/>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

