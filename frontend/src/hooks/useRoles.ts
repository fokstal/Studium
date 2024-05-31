export function useRoles() {
  const rolesString = localStorage.getItem("role");

  if (!rolesString) return [];
  const roles = JSON.parse(rolesString);
  return roles.map((role: any) => role.name);
}
