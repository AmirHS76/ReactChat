export interface UpdateUserRequest {
  id: number;
  username?: string;
  email?: string;
  isDisabled: boolean;
  role: string;
}
