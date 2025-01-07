import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'phone', standalone: true })
export class PhonePipe implements PipeTransform {
    formatPhoneNumber(s: string) {
        const m = s.match(/^(\d{1})(\d{3})(\d{3})(\d{2})(\d{2})$/);
        return !m ? null : `${m[1]} (${m[2]}) ${m[3]}-${m[4]}-${m[5]}`;
    }

    transform(val: string) {
        let formattedPhone: string | null = null;
        try {
            formattedPhone = this.formatPhoneNumber(val);
        } catch (error) {
            formattedPhone = val;
        }

        if (formattedPhone == null) formattedPhone = val;

        return formattedPhone;
    }
}
