import { StudentsTable } from "../data-tables";
import { Header } from "../header";
import "./default.css";

export function App() {
  return (
    <div className="App">
      <Header/>
      <StudentsTable/>
    </div>
  );
}

