class AccessData {
  accesses: string[]; // Store accesses as an array of strings

  constructor(accesses: string | string[]) {
    if (typeof accesses === "string") {
      if (accesses === "None") {
        this.accesses = [];
      } else {
        // Split by comma and trim spaces
        this.accesses = accesses.split(",").map((a) => a.trim());
      }
    } else if (Array.isArray(accesses)) {
      this.accesses = accesses;
    } else {
      throw new Error("Invalid access data format");
    }
  }

  static fromJson(json: string | null): AccessData | null {
    if (json === null) {
      return null;
    }
    const parsed = JSON.parse(json);
    return new AccessData(parsed.accesses);
  }
}
export default AccessData;
