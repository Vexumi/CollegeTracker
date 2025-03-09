import { AsyncPipe, CommonModule } from '@angular/common';
import { Component, DestroyRef, ElementRef, Input, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { ProjectModel } from '../../../../entities/project/project.model';
import { BehaviorSubject, tap } from 'rxjs';
import { ProjectAttachmentModel } from '../../../../entities/project/project-attachment.model';
import { ProjectService } from '../../../../entities/project/project.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
    standalone: true,
    selector: 'app-project-attachments-block',
    templateUrl: './attachments.component.html',
    styleUrls: ['./attachments.component.scss'],
    imports: [CommonModule, AsyncPipe]
})
export class ProjectAttachmentsBlockComponent implements OnChanges {
    @ViewChild('fileInput') fileInput!: ElementRef;

    @Input({ required: true })
    public project!: ProjectModel;

    public attachments$ = new BehaviorSubject<ProjectAttachmentModel[]>([]);

    constructor(
        private readonly projectService: ProjectService,
        private readonly destroyRef: DestroyRef
    ) {}

    public ngOnChanges(changes: SimpleChanges): void {
        if (changes['project'] && this.project.id !== undefined) {
            this.loadAttachments();
        }
    }

    public loadAttachments() {
        this.projectService
        .getAttachments(this.project.id)
        .pipe(
            takeUntilDestroyed(this.destroyRef),
            tap((x) => this.attachments$.next(x))
            )
        .subscribe();
    }

    public onAddClicked() {
        this.fileInput.nativeElement.click();
    }

    public openAttachment(attachment: ProjectAttachmentModel) {
        if (attachment.url != null) {
            window.open(attachment.url, "_blank");
            return;
        }

        this.projectService.downloadAndSaveFile(attachment.id, attachment.name);
    }

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    public uploadFile(event: any) {
        const fileToUpload = event.target.files[0];
        const formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);

        this.projectService.uploadFile(this.project.id, formData)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe(() => this.loadAttachments());
    }
}