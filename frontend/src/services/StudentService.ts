class StudentService {
  public static getStudents = async () => {
    const res = await fetch("https://freetestapi.com/api/v1/students");

    if (!res.ok) return [];

    return await res.json();
  }
}

export default StudentService;