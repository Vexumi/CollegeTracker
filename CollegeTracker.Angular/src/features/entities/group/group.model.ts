import { BaseModel } from "../base.model";
import { SpecialityModel } from "../speciality/speciality.model";

export interface GroupModel extends BaseModel {
    number: string,
    launchDate: Date,
    stopDate: Date,
    specialityId: number,
    speciality: SpecialityModel
}