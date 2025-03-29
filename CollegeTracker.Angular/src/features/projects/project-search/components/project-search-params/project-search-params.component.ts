import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { ProjectSearchParamsModel } from '../../../../entities/project/project-search-params.model';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ProjectStateEnum } from '../../../../entities/project/project-state.enum';
import { DropdownSelectIdComponent, DropdownSelectIdItem } from '../../../../../shared/component/dropdown/dropdown.component';
import { getProjectStateString } from '../../../../../shared/utils/project-state.utils';
import { SpecialityService } from '../../../../entities/speciality/speciality.service';
import { map, Observable } from 'rxjs';

@Component({
    standalone: true,
    selector: 'app-project-search-params',
    templateUrl: './project-search-params.component.html',
    styleUrls: ['./project-search-params.component.scss'],
    imports: [CommonModule, ReactiveFormsModule, DropdownSelectIdComponent]
})
export class ProjectSearchParamsComponent {
    @Output()
    public searchClicked = new EventEmitter<ProjectSearchParamsModel>();

    private projectStates = Object.values(ProjectStateEnum).splice(Object.keys(ProjectStateEnum).length / 2) as ProjectStateEnum[];
    private readonly emptyOption = { title: '', id: null } as DropdownSelectIdItem;

    public form = new FormGroup({
        title: new FormControl<string | null>(null),
        description: new FormControl<string | null>(null),
        state: new FormControl<ProjectStateEnum | null>(null),
        specialityId: new FormControl<number | null>(null),
        startDateFrom: new FormControl<Date | null>(null),
        startDateTo: new FormControl<Date | null>(null),
        actualEndDateFrom: new FormControl<Date | null>(null),
        actualEndDateTo: new FormControl<Date | null>(null),
        deadlineFrom: new FormControl< Date | null>(null),
        deadlineTo: new FormControl<Date | null>(null),
    });

    public specialityOptions$: Observable<DropdownSelectIdItem[]>;

    constructor(
        private readonly specialitiesService: SpecialityService
    ) {
        this.specialityOptions$ = this.specialitiesService.getAllActive()
            .pipe(
                map(res => res.map((x) => ({ title: x.title, id: x.id } as DropdownSelectIdItem)).concat([this.emptyOption]))
            );
    }

    public getProjectStateItems() {
        return this.projectStates.map((state, index) => ({
            title: getProjectStateString(state),
            id: index
        } as DropdownSelectIdItem)).concat([this.emptyOption]);
    }

    public onProjectStateSelected(item: DropdownSelectIdItem | null) {
        if (item && item.id) {
            this.form.controls.state.setValue(this.projectStates[item.id]);
            return;
        }
        this.form.controls.state.setValue(null);
    }

    public onSpecialitySelected(item: DropdownSelectIdItem | null) {
        this.form.controls.specialityId.setValue(item?.id ?? null);
    }

    public onSearchClicked() {
        this.searchClicked.emit(this.form.value as ProjectSearchParamsModel);
    }
}
