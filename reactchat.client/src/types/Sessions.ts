export default interface Session {
  id: number;
  userId: string;
  userAgent: string;
  ipAddress: string;
  createdAt: string;
  lastActivity: string;
  isRevoked: boolean;
}
