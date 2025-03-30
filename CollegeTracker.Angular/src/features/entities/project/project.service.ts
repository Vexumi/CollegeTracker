import { Injectable } from '@angular/core';
import { ApiEndpoints } from '../../../constants/api-routes';
import { BaseService } from '../base.service';
import { ProjectModel } from './project.model';
import { Observable, startWith } from 'rxjs';
import { ProjectAttachmentModel } from './project-attachment.model';
import { HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../../shared/services/auth.service';
import { ProjectStateEnum } from './project-state.enum';
import { UserRole } from '../user/user-role.model';
import { ProjectSearchParamsModel } from './project-search-params.model';


type StateTransitionsType = Record<ProjectStateEnum, ProjectStateEnum[]>;
type StatePermissionsType = Record<ProjectStateEnum, UserRole[]>;


@Injectable({
    providedIn: 'root'
})
export class ProjectService extends BaseService<ProjectModel> {
    private readonly StateTransitions: StateTransitionsType = {
        [ProjectStateEnum.Created]: [ProjectStateEnum.InProgress],
        [ProjectStateEnum.InProgress]: [ProjectStateEnum.OnReview],
        [ProjectStateEnum.OnReview]: [ProjectStateEnum.Rejected, ProjectStateEnum.Reviewed],
        [ProjectStateEnum.Reviewed]: [ProjectStateEnum.Rejected, ProjectStateEnum.Completed],
        [ProjectStateEnum.Rejected]: [ProjectStateEnum.InProgress],
        [ProjectStateEnum.Completed]: [],
    };

    private readonly StatePermissions: StatePermissionsType = {
        [ProjectStateEnum.Created]: [UserRole.Admin],
        [ProjectStateEnum.InProgress]: [UserRole.Teacher, UserRole.Admin],
        [ProjectStateEnum.OnReview]: [UserRole.Student, UserRole.Teacher, UserRole.Admin],
        [ProjectStateEnum.Reviewed]: [UserRole.Teacher, UserRole.Admin],
        [ProjectStateEnum.Rejected]: [UserRole.Teacher, UserRole.Admin],
        [ProjectStateEnum.Completed]: [UserRole.Teacher, UserRole.Admin],
    };

    constructor(private readonly authService: AuthService) {
        super(ApiEndpoints.Projects);
    }

    public searchProjects(searchParams?: ProjectSearchParamsModel): Observable<ProjectModel[]> {
        return this.http.post<ProjectModel[]>(`${this.baseControllerUrl}/Search`, searchParams).pipe(startWith([]));
    }

    public changeState(projectId: number, state: ProjectStateEnum) {
        return this.http.post(`${this.baseControllerUrl}/ChangeState/${projectId}`, state);
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
        const currentUser = this.authService.getCurrentUser();
        return project.students.some(x => x.userInfo.id === currentUser.id) || 
            project.teacher.userInfo.id === currentUser.id || 
            currentUser.role === UserRole.Admin;
    }

    public getSuitableStates(project: ProjectModel) {
        const currentUserRole = this.authService.getCurrentUser().role;

        if (this.authService.isAdmin()) return Object.values(ProjectStateEnum).splice(Object.keys(ProjectStateEnum).length / 2) as ProjectStateEnum[];

        const transitions = this.StateTransitions[project.state];

        return transitions.filter((state) => {
            const permissions = this.StatePermissions[state];
            return permissions.includes(currentUserRole);
        });
    }
}