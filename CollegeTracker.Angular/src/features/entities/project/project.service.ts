import { Injectable } from '@angular/core';
import { ApiEndpoints } from '../../../constants/api-routes';
import { BaseService } from '../base.service';
import { ProjectModel } from './project.model';
import { Observable } from 'rxjs';
import { ProjectAttachmentModel } from './project-attachment.model';
import { HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../../shared/services/auth.service';

@Injectable({
    providedIn: 'root'
})
export class ProjectService extends BaseService<ProjectModel> {
    constructor(private readonly authService: AuthService) {
        super(ApiEndpoints.Projects);
    }

    public getById(id: number): Observable<ProjectModel> {
        return this.http.get<ProjectModel>(`${this.baseControllerUrl}/GetById/${id}`);
    }

    public getAttachments(projectId: number): Observable<ProjectAttachmentModel[]> {
        return this.http.get<ProjectAttachmentModel[]>(`${this.baseControllerUrl}/GetAttachments/${projectId}`);
    }

    public addLink(attachment: ProjectAttachmentModel) {
        return this.http.post(`${this.baseControllerUrl}/AddLink`, attachment);
    }

    public uploadFile(projectId: number, file: FormData) {
        return this.http.post(`${this.baseControllerUrl}/UploadFile/${projectId}`, file);
    }

    public downloadFile(attachmentId: number) {
        const url = `${this.baseControllerUrl}/DownloadFile/${attachmentId}`;
        const headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': 'application/octet-stream'
          });
        return this.http.get(url, { headers: headers, responseType: 'blob' });
    }

    public downloadAndSaveFile(attachmentId: number, fileName: string): void {
        this.downloadFile(attachmentId).subscribe(blob => {
            const url = window.URL.createObjectURL(blob);

            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', fileName);
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            window.URL.revokeObjectURL(url);
        });
    }

    public userIsParticipantOfProject(project: ProjectModel) {
        const currentUserId = this.authService.getCurrentUser().id;
        return project.students.some(x => x.userInfo.id === currentUserId) || project.teacher.id === currentUserId;
    }
}