import { PropsWithChildren, useState } from 'react';
import { Pressable, Text, View } from 'react-native';
import { styles } from '@/styles/accordion.styles';

type AccordionSectionProps = PropsWithChildren<{
  title: string;
  defaultOpen?: boolean;
  subtitle?: string;
}>;

export function AccordionSection({
  title,
  defaultOpen = false,
  subtitle,
  children,
}: AccordionSectionProps) {
  const [open, setOpen] = useState(defaultOpen);

  return (
    <View style={styles.block}>
      <Pressable style={styles.header} onPress={() => setOpen((current) => !current)}>
        <View style={styles.headerText}>
          <Text style={styles.blockTitle}>{title}</Text>
          {subtitle ? <Text style={styles.subtitle}>{subtitle}</Text> : null}
        </View>
        <Text style={styles.indicator}>{open ? '−' : '+'}</Text>
      </Pressable>

      {open ? <View style={styles.content}>{children}</View> : null}
    </View>
  );
}
