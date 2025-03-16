import { AsyncPipe, CommonModule } from '@angular/common';
import { Component, DestroyRef, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ProjectModel } from '../../../../entities/project/project.model';
import { BehaviorSubject, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatDialog } from '@angular/material/dialog';
import { MessageModel } from '../../../../entities/message/message.model';
import { MessageService } from '../../../../entities/message/message.service';
import {MatCardModule} from '@angular/material/card';
import { MessageUserComponent } from './components/message-user/message-user.component';

@Component({
    standalone: true,
    selector: 'app-messages-block',
    templateUrl: './messages.component.html',
    styleUrls: ['./messages.component.scss'],
    imports: [CommonModule, AsyncPipe, MatCardModule, MessageUserComponent]
})
export class MessagesBlockComponent implements OnChanges {
    @Input({ required: true })
    public project!: ProjectModel;

    public messages = new BehaviorSubject<MessageModel[]>([]);

    constructor(
        private readonly messageService: MessageService,
        private readonly destroyRef: DestroyRef,
        private readonly dialogService: MatDialog
    ) {}

    public ngOnChanges(changes: SimpleChanges): void {
        if (changes['project'] && this.project.id !== undefined) {
            this.loadMessages();
        }
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
}