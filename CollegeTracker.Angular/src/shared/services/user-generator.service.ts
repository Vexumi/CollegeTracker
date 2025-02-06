import { Injectable } from '@angular/core';
import { transliterate as tr, slugify } from 'transliteration';
import _ from 'lodash';

@Injectable({
  providedIn: 'root'
})
export class UserGeneratorService {
  readonly lowercaseChars = 'abcdefghijklmnopqrstuvwxyz';
  readonly uppercaseChars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
  readonly numberChars = '0123456789';
  // eslint-disable-next-line no-useless-escape
  readonly symbolChars = '!@#$%^&*()_+~`|}{[]\:;?><,./-=';
  readonly allChars = this.lowercaseChars + this.uppercaseChars + this.numberChars + this.symbolChars;
  readonly targetPasswordLength = 8;

  public generateUserName(fullName: string): string | null {
    const parts = fullName.split(' ');
    if (parts.length !== 3) {
      return null;
    }

    const lastName = tr(parts[0]);
    const firstNameInitial = tr(parts[1].charAt(0));
    const patronymicInitial = tr(parts[2].charAt(0));
    const randomNumber = _.random(100, 999);

    return `${this.slugifyText(lastName)}${this.slugifyText(firstNameInitial)}${this.slugifyText(patronymicInitial)}${randomNumber}`;
  }

  public generatePassword() {
    let password = '';
    for (let i = 0; i < this.targetPasswordLength; i++) {
      const randomIndex = Math.floor(Math.random() * this.allChars.length);
      password += this.allChars.charAt(randomIndex);
    }

    return password;
  }

  private slugifyText(text: string): string {
      return slugify(text, { lowercase: false, separator: '' });
  }
}