import { ExhibitionProfileVM } from './ExhibitionProfileVM';
import { UserVM } from './UserVM';

export interface ProfileVM {
  firstName: string;
  lastName: string;
  userName: string;
  profilePicture: string;
  email: string;
  phone: string;
  biography: string;
  followers: UserVM[];
  following: UserVM[];
  exhibitions: ExhibitionProfileVM[];
}
