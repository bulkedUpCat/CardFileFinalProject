import { TextMaterial } from "../TextMaterial";

export interface User{
  id: string;
  userName: string;
  email: string;
  password: string;
  textMaterials: Array<TextMaterial>;
  roles: Array<string>;
  receiveNotifications: boolean;
}
