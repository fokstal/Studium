import { useParams } from "react-router-dom";
import { StudentComponent } from "../../components/students";
import { StudentPageLayout } from "../../layouts";

export function Student() {
  const { id } = useParams();
  return (
    <StudentPageLayout>
      <StudentComponent id={id || ""} />
    </StudentPageLayout>
  );
}
