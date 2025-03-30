import AccessData from "../contexts/AccessData";
import UserRepository from "../Repositories/UserRepository";

const userRepo = new UserRepository();
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
  checkAccesses: async (accesses: string[]) => {
    let userAccesses = AccessData.fromJson(localStorage.getItem("accesses"));
    if (userAccesses === null || userAccesses.accesses === null) {
      userAccesses = await setUserAccesses();
    }
    if (userAccesses === null) {
      return false;
    }
    return accesses.some((access) => userAccesses.accesses.includes(access));
  },
};
const setUserAccesses = async (): Promise<AccessData | null> => {
  const accesses = localStorage.getItem("accesses");
  if (accesses !== null) {
    return AccessData.fromJson(accesses);
  }
  const response = await userRepo.getAccesses();
  localStorage.setItem("accesses", JSON.stringify(response.data));
  return AccessData.fromJson(JSON.stringify(response.data));
};
export default AccessesService;
