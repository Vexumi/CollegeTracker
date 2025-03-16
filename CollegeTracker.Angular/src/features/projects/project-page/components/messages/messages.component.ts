import { AsyncPipe, CommonModule } from '@angular/common';
import { AfterViewChecked, Component, DestroyRef, ElementRef, Input, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { ProjectModel } from '../../../../entities/project/project.model';
import { BehaviorSubject, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MessageModel } from '../../../../entities/message/message.model';
import { MessageService } from '../../../../entities/message/message.service';
import {MatCardModule} from '@angular/material/card';
import { MessageUserComponent } from './components/message-user/message-user.component';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MessageDtoModel } from '../../../../entities/message/message-dto.model';
import { AuthService } from '../../../../../shared/services/auth.service';

@Component({
    standalone: true,
    selector: 'app-messages-block',
    templateUrl: './messages.component.html',
    styleUrls: ['./messages.component.scss'],
    imports: [CommonModule, AsyncPipe, MatCardModule, MessageUserComponent, ReactiveFormsModule]
})
export class MessagesBlockComponent implements OnChanges, AfterViewChecked {
    @ViewChild('chatScroll') private scrollContainer!: ElementRef;

    @Input({ required: true })
    public project!: ProjectModel;

    public messages = new BehaviorSubject<MessageModel[]>([]);

    public formControl = new FormControl<string | null>(null, [Validators.required]);

    constructor(
        private readonly messageService: MessageService,
        private readonly authService: AuthService,
        private readonly destroyRef: DestroyRef,
    ) {}

    public ngOnChanges(changes: SimpleChanges): void {
        if (changes['project'] && this.project.id !== undefined) {
            this.loadMessages();
        }
    }

    public ngAfterViewChecked() {        
        this.scrollToBottom();        
    } 

    scrollToBottom(): void {
        this.scrollContainer.nativeElement.scrollIntoView();           
    }

    public loadMessages() {
        this.messageService
        .getAllByProject(this.project.id)
        .pipe(
            takeUntilDestroyed(this.destroyRef),
            tap((x) => this.messages.next(x))
        )
        .subscribe();
    }

    public sendMessage() {
        const message = {
            senderId: this.authService.getCurrentUser().id,
            projectId: this.project.id,
            content: this.formControl.value
        } as MessageDtoModel;

        this.formControl.setValue(null);

        this.messageService.create(message as MessageModel)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe(() => this.loadMessages());
    }
}