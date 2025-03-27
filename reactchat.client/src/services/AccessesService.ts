import AccessData from "../contexts/AccessData";

const AccessesService = {
  checkAccess: () => {
    const accesses = AccessData.fromJson(localStorage.getItem("accesses"));
    if (
      accesses === null ||
      accesses.accesses === null ||
      accesses.accesses.length === 0
    ) {
      return false;
    }
    return true;
  },
  checkAccesses: (accesses: string[]) => {
    const userAccesses = AccessData.fromJson(localStorage.getItem("accesses"));
    if (userAccesses === null || userAccesses.accesses === null) {
      return false;
    }
    return accesses.some((access) => userAccesses.accesses.includes(access));
  },
};

export default AccessesService;
