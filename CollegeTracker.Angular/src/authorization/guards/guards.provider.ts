import { Provider } from "@angular/core";
import { AdminGuard } from "./admin.guard";
import { TeacherGuard } from "./teacher.guard";
import { StudentGuard } from "./student.guard";
import { SignedInGuard } from "./signed-in.guard";

export const guardsProvider: Provider = [
    AdminGuard,
    TeacherGuard,
    StudentGuard,
    SignedInGuard
];