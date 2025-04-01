import { inject, Injectable } from '@angular/core';
import { ApiEndpoints } from '../../../constants/api-routes';
import { ProjectSearchParamsModel } from '../project/project-search-params.model';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { saveAs } from 'file-saver';
import { firstValueFrom } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class ReportsService {
    protected readonly http = inject(HttpClient);
    protected readonly baseControllerUrl = ApiEndpoints.Reports;

    public async exportProjects(searchParams?: ProjectSearchParamsModel) {
        const result = await firstValueFrom(
            this.http.post(`${this.baseControllerUrl}/ExportProjects`, searchParams, {
                responseType: 'blob',
                observe: 'response'
            })
        );
        this.saveBlobToFile(result);
    }

    private saveBlobToFile(result: HttpResponse<Blob>) {
        if (!result.body || result.body.size == 0) {
            console.error("No data received from export endpoint.");
            return;
        }
        const contentDispositionHeader = result.headers.get('content-disposition');
        const filename = this.getFileNameFromHeader(contentDispositionHeader);
        saveAs(result.body, filename ?? 'report.xlsx');
    }

    private getFileNameFromHeader(contentDispositionHeader: string | null): string {
        if (!contentDispositionHeader) {
          return 'report.xlsx';
        }
      
        const filenameRegex = /filename\*=UTF-8''([\w\-%.]+)|filename=(["']?)([\w\-%.]+)\2/;
        const matches = filenameRegex.exec(contentDispositionHeader);
      
        if (matches) {
            if (matches[1]) {
                try {
                    return decodeURIComponent(matches[1]);
                } catch (e) {
                    console.warn("Failed to decode filename", e);
                    return 'report.xlsx';
                }
            }

            if (matches[3]) {
                return matches[3];
            }
        }
        return 'report.xlsx';
      }
    
}