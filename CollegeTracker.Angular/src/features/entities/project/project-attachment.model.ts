import { BaseModel } from "../base.model";

export interface ProjectAttachmentModel extends BaseModel {
    name: string,
    url: string | null,
    filePath: string | null
}