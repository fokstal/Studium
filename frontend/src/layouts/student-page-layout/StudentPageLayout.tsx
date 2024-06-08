import { VStack } from "@chakra-ui/react";
import { BaseLayout } from "../base-layout";
import { colors } from "../../components/ui-kit";
import { ReactNode } from "react";

type StudentPageLayoutProps = {
  children: ReactNode;
};

export function StudentPageLayout({ children }: StudentPageLayoutProps) {
  return (
    <BaseLayout bg={colors.darkGrey}>
      <VStack
        p="80px 100px"
        minH="calc(100vh - 100px)"
        align="stretch"
        gap="20px"
      >{children}</VStack>
    </BaseLayout>
  );
}
