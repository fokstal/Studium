import { Box, Container, Flex } from "@chakra-ui/react";
import { colors } from "../../components/ui-kit/variables";

type BaseLayoutProps = {
  children: React.ReactNode;
  bg?: string;
}

export function BaseLayout({children, bg = colors.white}: BaseLayoutProps) {
  return (
    <Box background={bg}>
      <Container maxW="1440px" p="0 100px" boxSizing="border-box" m="auto">
          {children}
      </Container>
    </Box>
  )
}