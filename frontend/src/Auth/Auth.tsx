import { LoginComponent } from "./components/LoginComponent/LoginComponent";
import { RegisterComponent } from "./components/RegisterComponent/RegisterComponent";
import { useState } from "react";

export default function Auth() {
  const [view, setView] = useState<string>("LOGIN");

  if (view === "LOGIN") return <LoginComponent setView={setView} />;
  if (view === "REGISTER") return <RegisterComponent setView={setView} />;
  else return <LoginComponent setView={setView} />;
}
