import AccessData from "../contexts/AccessData";
import UserRepository from "../Repositories/UserRepository";

const userRepo = new UserRepository();
const AccessesService = {
  checkAccess: async () => {
    const accesses = await setUserAccesses();
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
    const userAccesses = await setUserAccesses();
    if (userAccesses === null) {
      return false;
    }
    return accesses.some((access) => userAccesses.accesses.includes(access));
  },
};
const setUserAccesses = async (): Promise<AccessData | null> => {
  const response = await userRepo.getAccesses();
  return AccessData.fromJson(JSON.stringify(response.data));
};
export default AccessesService;
