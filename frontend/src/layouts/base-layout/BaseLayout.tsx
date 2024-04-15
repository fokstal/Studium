import { Box, Container, Flex } from "@chakra-ui/react";
import { colors } from "../../components/ui-kit/variables";

type BaseLayoutProps = {
  children: React.ReactNode;
}

export function BaseLayout({children}: BaseLayoutProps) {
  return (
    <Box background={colors.white}>
      <Container maxW="1440px" p="0 100px" h="100px" boxSizing="border-box" m="auto">
          {children}
      </Container>
    </Box>
  )
}