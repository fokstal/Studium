export function useRoles() {
  const rolesString = localStorage.getItem("role");

  if (!rolesString || rolesString === "undefined") return [];
  const roles = JSON.parse(rolesString);
  return roles.map((role: any) => role.name);
}
