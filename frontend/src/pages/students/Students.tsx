import { useParams } from "react-router-dom";
import { StudentComponent } from "../../components/students";
import { StudentPageLayout } from "../../layouts";

export function Students() {
  const { id } = useParams();
  return (
    <StudentPageLayout>
      <StudentComponent id={+(id || 0)} />
    </StudentPageLayout>
  );
}
