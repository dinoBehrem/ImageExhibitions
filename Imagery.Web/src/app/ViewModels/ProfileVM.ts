import { ExhibitionProfileVM } from './ExhibitionProfileVM';
import { UserVM } from './UserVM';

export interface ProfileVM {
  profilePicture: string;
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  phone: string;
  biography: string;
  followers: UserVM[];
  following: UserVM[];
  exhibitions: ExhibitionProfileVM[];
  roles: string[];
}
