export interface User {
  id?: string;  // MongoDB's default field for ID
  userName: string;
  email: string;
  phone?: string;
  address?: string;
  city?: string;
  consentGiven: boolean;
  canCreateUsers: boolean;
  canDeleteUsers: boolean;
  canEditUsers: boolean;
}
