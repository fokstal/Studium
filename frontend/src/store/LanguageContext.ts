import { Dispatch, SetStateAction, createContext } from "react";
import { Translator } from "./Translator";

export const LanguageContext = createContext<{
  lang: {name: keyof typeof Translator};
  setLang: Dispatch<SetStateAction<{name: keyof typeof Translator}>>;
}>({ lang: {name: "RU"}, setLang: () => {} });
