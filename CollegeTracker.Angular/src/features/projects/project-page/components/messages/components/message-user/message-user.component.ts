import { CommonModule, DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MessageModel } from '../../../../../../entities/message/message.model';
import { AuthService } from '../../../../../../../shared/services/auth.service';

@Component({
    standalone: true,
    selector: 'app-message-user',
    templateUrl: './message-user.component.html',
    styleUrls: ['./message-user.component.scss'],
    imports: [CommonModule, DatePipe]
})
export class MessageUserComponent {
    @Input({ required: true })
    public message!: MessageModel;

    public currentUserId: number;

    constructor(private readonly authService: AuthService) {
        this.currentUserId = this.authService.getCurrentUser().id;
    }
}