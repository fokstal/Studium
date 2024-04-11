import { Button } from "../ui-kit/button/button";
import "./default.css";

export function App() {
  return (
    <div className="App">
      <div style={{display: "flex", gap: "10px", margin: "30px"}}>
      <Button>Go lbutton</Button>
      <Button>Go lbutton</Button>
      <Button disabled>Disabled</Button>
      </div>
    </div>
  );
}

